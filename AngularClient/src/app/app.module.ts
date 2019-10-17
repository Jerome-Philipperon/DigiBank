import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './Layout/header/header.component';
import { FooterComponent } from './Layout/footer/footer.component';
import { LoginComponent } from './login/login.component';
import { InformationsComponent } from './informations/informations.component';
import { AccountsComponent } from './accounts/accounts.component';
import { RibComponent } from './accounts/rib/rib.component';
import { TransferComponent } from './accounts/transfer/transfer.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { DetailsComponent } from './accounts/details/details.component';
import { ContactComponent } from './contact/contact.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    InformationsComponent,
    AccountsComponent,
    RibComponent,
    TransferComponent,
    NotFoundComponent,
    DetailsComponent,
    ContactComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
