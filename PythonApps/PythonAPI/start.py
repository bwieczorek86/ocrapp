# -*- coding: utf8 -*-
import base64

from logging.config import dictConfig
import logging
from fastapi import FastAPI, File, UploadFile
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from pdf2image import convert_from_path, convert_from_bytes
from typing import List, Optional
import spylls.hunspell
import tempfile
import uvicorn


class LogConfig(BaseModel):
    """Logging configuration to be set for the server"""

    LOGGER_NAME: str = "mycoolapp"
    LOG_FORMAT: str = "%(levelprefix)s | %(asctime)s | %(message)s"
    LOG_LEVEL: str = "DEBUG"

    # Logging config
    version = 1
    disable_existing_loggers = False
    formatters = {
        "default": {
            "()": "uvicorn.logging.DefaultFormatter",
            "fmt": LOG_FORMAT,
            "datefmt": "%Y-%m-%d %H:%M:%S",
        },
    }
    handlers = {
        "default": {
            "formatter": "default",
            "class": "logging.StreamHandler",
            "stream": "ext://sys.stderr",
        },
    }
    loggers = {
        "mycoolapp": {"handlers": ["default"], "level": LOG_LEVEL},
    }


dictConfig(LogConfig().dict())
logger = logging.getLogger("mycoolapp")

app = FastAPI()

default_dict = spylls.hunspell.Dictionary.from_files('dicts/pl_PL')
names_dict = spylls.hunspell.Dictionary.from_files('dicts/names')
surnames_dict = spylls.hunspell.Dictionary.from_files('dicts/surname')
cities_dict = spylls.hunspell.Dictionary.from_files('dicts/cities')
streets_dict = spylls.hunspell.Dictionary.from_files('dicts/streets')

app.add_middleware(
    CORSMiddleware,
    allow_origins="*",
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


class ConvertToPdf(BaseModel):
    PdfFilePath: str
    ImagePath: str


class WordsToCheck(BaseModel):
    PropertyName: str
    Text: str
    CorrectedText: Optional[str] = ""


def get_first_suggestion(spylls_dict, word):
    for suggestion in spylls_dict.suggest(word):
        return suggestion


def get_specific_dict(prop_name):
    if prop_name == "Name":
        return names_dict
    elif prop_name == "Surname":
        return surnames_dict
    elif prop_name == "City":
        return cities_dict
    elif prop_name == "Street":
        return streets_dict
    else:
        return default_dict


def spylls_correct(words_list: List[WordsToCheck]):
    for prop in words_list:

        specific_dict = get_specific_dict(prop.PropertyName)
        splitted_words = prop.Text.split()
        if len(splitted_words) > 1:
            corrected_sentence = []
            for word in splitted_words:
                if not specific_dict.lookup(word):
                    if get_first_suggestion(specific_dict, word) is not None:
                        corrected_sentence.append(get_first_suggestion(specific_dict, word))
                    else:
                        corrected_sentence.append(word)
                else:
                    corrected_sentence.append(word)
            prop.CorrectedText = " ".join(corrected_sentence)
        else:
            if specific_dict.lookup(prop.Text) is False:
                # False
                for suggestion in specific_dict.suggest(prop.Text):
                    prop.CorrectedText = suggestion

    return words_list


@app.get("/")
def read_root():
    return {"Hello": "World"}


@app.post("/check-words/")
def check_words(words_list: List[WordsToCheck]):
    corrected_list = spylls_correct(words_list)
    return corrected_list


@app.post("/convert-pdf-bytes/")
async def convert_pdf_bytes_to_image(pdfBytes: UploadFile = File(...)):
    try:
        file = pdfBytes.file.read()
        pages = convert_from_bytes(file, last_page=1, first_page=0)

        return pages[0]
    except ValueError:
        result = 'Fuckup'
        return result


@app.post("/convert-pdf/")
async def convert_pdf_to_image(convertToPdf: ConvertToPdf):
    try:
        with tempfile.TemporaryDirectory() as path:
            images_from_path = convert_from_path(convertToPdf.PdfFilePath, output_folder=path, last_page=1, first_page=0)

            for page in images_from_path:
                page.save(convertToPdf.ImagePath, 'JPEG')

            result = 'File Converted Correctly'
        return result
    except ValueError:
        result = 'Fuckup'
        return result


@app.post("/convert-pdf/")
async def convert_pdf_to_image():
    try:
        with tempfile.TemporaryDirectory() as path:
            images_from_path = convert_from_path(r"C:\Users\bwita\OneDrive\Desktop\Prezentacja\Wszsczęcie egzekucji\Skan (13).pdf", output_folder=path, last_page=1, first_page=0)

            for page in images_from_path:
                page.save(r"C:\Users\bwita\OneDrive\Desktop\Prezentacja\Wszsczęcie egzekucji\Skan (13)", 'JPEG')

            result = 'File Converted Correctly'
        return result
    except ValueError:
        result = 'Fuckup'
        return result

if __name__ == "__main__":
    uvicorn.run("main:app", host="127.0.0.1", port=8000, log_level="info")
