const path = require("path");

module.exports = {
    module: {
        rules: [
            {
                test: require('path').resolve(__dirname, 'node_modules/leader-line/'),
                use: [{
                  loader: 'skeleton-loader',
                  options: {procedure: content => `${content}export default LeaderLine`}
                }]
            },
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            }
        ]
    },
    output: {
        path: path.resolve(__dirname, '../wwwroot/js'),
        filename: "my_lib.js",
        library: "MyLib"
    }
};