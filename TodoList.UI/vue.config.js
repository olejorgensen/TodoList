const fs = require('fs')
const path = require('path')

module.exports = {
    devServer: {
        proxy: {
            '^/TodoList': {
                target: 'http://localhost:5150'
            }
        },
        port: 5150
    }
}