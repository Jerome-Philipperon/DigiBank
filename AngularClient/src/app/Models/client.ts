import  Account  from './account';

export default class Client {
    id : string;
    firstName : string;
    lastName : string;
    dateOfBirth : Date;
    street : string;
    zipCode : number;
    city : string;
    myAccounts : Account[];
    email : string;
}
