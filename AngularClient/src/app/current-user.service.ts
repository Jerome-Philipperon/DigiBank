import { Injectable } from '@angular/core';
import Client from './Models/client';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  currentUser : Client;

  constructor() { }

  public getCurrentUser ()
  {
    return this.getCurrentUser;
  }

  public setCurrentUser (user : Client)
  {
    this.currentUser = user;
  }
}
