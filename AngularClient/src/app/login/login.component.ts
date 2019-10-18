import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CurrentUserService } from '../current-user.service';
import Client from '../Models/client';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  firstName : string;
  lastName : string;
  
  constructor(private http : HttpClient, private currentUser : CurrentUserService) { }

  ngOnInit() {
  }

  callApi() {
    this.http.get(`https://localhost:44310/api/clients/${this.lastName}/${this.firstName}`).subscribe((data : Client) => {
      console.log(data);
      this.currentUser.setCurrentUser(data);
    })
  }

}
