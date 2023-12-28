import LeaderLine from '../node_modules/leader-line';

export function drawLeaderLine(startElementId, endElementId) {
    if (!window.lines){
        window.lines = {};
    }

    if (window.lines[startElementId]) {
        let oldLine = window.lines[startElementId];
        // RemoveEventHandlers(startElementId, oldLine)
        oldLine.remove();
    }

    const line = createLine(startElementId, endElementId);
    window.lines[startElementId] = line;

    // CreateEventHandlers(startElementId, line);
}

function createLine(startElementId, endElementId) {
    return new LeaderLine(
        LeaderLine.mouseHoverAnchor(
            document.getElementById(startElementId),
            'draw',
            {
                style: {
                    backgroundImage: null,
                    backgroundColor: null,
                    left: "2px",
                    top: "2px",
                    bottom: null,
                    right: null
                },
                hoverStyle: {
                    backgroundColor: null
                },
            }),
        document.getElementById(endElementId),
        {
            color: '#5355AF',
            size: 3,
            dash: {animation: true},
            hide: true,
        }
    );
}

function RemoveEventHandlers(startElementId, oldLine) {
    let element = document.getElementById(startElementId);

    element.removeEventListener('mouseover', oldLine.show(), false);
    element.removeEventListener('mouseout', oldLine.hide(), false);
}

function CreateEventHandlers(startElementId, line) {
    let element = document.getElementById(startElementId);

    element.addEventListener('mouseover', line.show(), false);
    element.addEventListener('mouseout', line.hide(), false);
}