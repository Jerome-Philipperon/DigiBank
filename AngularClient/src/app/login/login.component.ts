import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  firstName : string;
  lastName : string;
  
  constructor(private http : HttpClient) { }

  ngOnInit() {
  }

  callApi() {
    console.log(this.firstName);
    console.log(this.lastName);
    this.http.get(`https://localhost:44310/api/clients/${this.lastName}/${this.firstName}`).subscribe((data) => {
      console.log(data)
    })
  }

}
