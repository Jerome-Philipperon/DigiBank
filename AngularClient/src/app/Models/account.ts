import Client from './client';

export default class Account {
    accountId : string;
    bankCode : string;
    branchCode : string;
    accountNumber : string;
    Key : string;
    IBAN : string;
    BIC : string;
    balance : number;
    accountOwner : Client;
    type : string;
}
