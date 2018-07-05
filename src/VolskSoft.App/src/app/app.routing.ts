const routes: Routes = [
  { path: '', component: EntryComponent,
  { path: '**' redirectTo: '' } //TODO introduce page not found
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [ RouterModule ]
})
export class AppRoutingModule
