import Account from './account';

export default class Deposit extends Account{
    creationDate : Date;
    autorizedOverdraft : number;
    freeOverdraft : number;
    overdraftChargeRate : number;
}
