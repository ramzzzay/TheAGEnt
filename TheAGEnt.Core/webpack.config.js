/// <binding />
"use strict";
var WebpackNotifierPlugin = require('webpack-notifier');
var Path = require('path');
var Webpack = require("webpack");
var ExtractTextPlugin = require('extract-text-webpack-plugin');
require('es6-promise').polyfill();

var AppDir = Path.resolve(__dirname, 'Assets/js');

module.exports = {
    entry: {
        application: [
            './Assets/css/site.css',
            './Assets/js/index.jsx']
    },
    output: {
        filename: "./Assets/bundle.js"
    },
    module: {
        loaders: [
          {
              test: /\.jsx?/,
              include: AppDir,
              loader: 'babel',
              query: {
                  presets: ['es2015', 'react']
              }
          },
          { test: /\.css$/, loader: ExtractTextPlugin.extract("style-loader", "css-loader") },
      { test: /.(jpg|png)(\?[a-z0-9=\.]+)?$/, loader: 'url-loader?limit=100000&name=Assets/imgs/[name].[ext]' },
      { test: /.(woff(2)?|eot|ttf|svg)(\?[a-z0-9=\.]+)?$/, loader: 'url-loader?limit=100000&name=Assets/fonts/[name].[ext]' },
      { test: /\.json$/, loader: "json" }

        ]
    },
    devtool: 'source-map',
    plugins: [
  new WebpackNotifierPlugin(),
  new Webpack.optimize.DedupePlugin(),
    new Webpack.ProvidePlugin({
        React: "react",
        ReactDOM: "react-dom",
        Cookie: "react-cookie"
    }),
  new ExtractTextPlugin('Assets/bundle.css')
    ],
    resolve: {
        extensions: ['', '.js', '.jsx']
    }
};