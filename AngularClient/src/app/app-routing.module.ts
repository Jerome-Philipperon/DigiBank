import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { InformationsComponent } from './informations/informations.component';
import { ContactComponent } from './contact/contact.component';
import { AccountsComponent } from './accounts/accounts.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { DetailsComponent } from './accounts/details/details.component';
import { TransferComponent } from './accounts/transfer/transfer.component';
import { RibComponent } from './accounts/rib/rib.component';


const routes: Routes = [
  {
    path: "login",
    component: LoginComponent
  },
  {path: "", redirectTo: "login", pathMatch : "full"},
  {path: "informations", component: InformationsComponent},
  {
    path : "accounts",
    children : 
    [
      {path : "", component : AccountsComponent},
      {path : "details/:id", component : DetailsComponent},
      {path : "transfer", component : TransferComponent},
      {path : "rib", component : RibComponent},
    ],
  },
  {path: "contact", component: ContactComponent},
  {path: "**", component: NotFoundComponent},
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
