const buildConfig = webpackConfig(): WebpackConfig => {
  const config: WebpackConfig = Object.assign({});
  
  config.module = {
    devtool: '', //TODO choose source map
    rules: [
      {
        test: //,
        loader: '',
        exclude: ''
      }
    ]
  }
};

module.exports = buildConfig;
