@NgModule({
  bootstrap: [ AppComponent ],
  declarations: [ AppComponent, APP_DECLARATIONS ],
  entryComponents: [APP_ENTRY_COMPONENTS],
  exports: [ AppComponent ],
  imports: [
    CommonModule,
    HttpClientModule,
    APP_IMPORTS,
    BrowserAnimationsModule,
    RouterModule.forRoot(routes, { useHash: false })
  ],
  providers: [
    ENV_PROVIDERS,
    APP_PROVIDERS
  ]
})
export class AppModule {
  constructor(
    public appRef: ApplicationRef,
    private _store: Store: Store<AppState>
  ){}
}
