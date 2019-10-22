import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import {FormsModule} from "@angular/forms";
import { AppComponent } from './app.component';
import { HeaderComponent } from './Layout/header/header.component';
import { FooterComponent } from './Layout/footer/footer.component';
import { LoginComponent } from './login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { InformationsComponent } from './informations/informations.component';
import { AccountsComponent } from './list/accounts/accounts.component';
import { RibComponent } from './list/accounts/rib/rib.component';
import { TransferComponent } from './list/accounts/transfer/transfer.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { DetailsComponent } from './list/accounts/details/details.component';
import { ContactComponent } from './contact/contact.component';
import { CurrentUserService } from './current-user.service';
import { ListComponent } from './list/list.component';

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
    ContactComponent,
    ListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
  ],
  providers: [
    CurrentUserService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
