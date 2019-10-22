import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { InformationsComponent } from './informations/informations.component';
import { ContactComponent } from './contact/contact.component';
import { AccountsComponent } from './list/accounts/accounts.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { DetailsComponent } from './list/accounts/details/details.component';
import { TransferComponent } from './list/accounts/transfer/transfer.component';
import { RibComponent } from './list/accounts/rib/rib.component';
import { ListComponent } from './list/list.component';


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
      {path : "", component : ListComponent},
      {path : "details/:id", component : DetailsComponent},
      {path : "transfer/:id", component : TransferComponent},
      {path : "rib/:id", component : RibComponent},
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
