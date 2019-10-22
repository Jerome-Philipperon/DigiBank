import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CurrentUserService } from '../current-user.service';

@Component({
  selector: 'bank-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  accountsList : Account[];

  constructor(private http : HttpClient, private currentUser : CurrentUserService) { }

  ngOnInit() {
    this.callApi();
  }

  callApi() {
    this.http.get(`https://localhost:44310/api/clients/${this.currentUser.getCurrentUser().id}/Account`).subscribe((data : Account[]) => {
      
      this.accountsList = data;
    })
  }
}
