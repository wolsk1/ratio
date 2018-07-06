import './polyfills.browser';
import './rxjs.imports';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { enableProdMode } from '@angular/core';

if(PRODUCTION){
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule);
