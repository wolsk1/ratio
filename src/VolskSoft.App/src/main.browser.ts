import './polyfills.browser';
import './rxjs.imports';

if(PRODUCTION){
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule);
