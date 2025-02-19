// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

export class User {
  // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
  constructor(id?: string, userName?: string, fullName?: string, email?: string, phoneNumber?: string, roles?: string[]) {

    this.id = id;
    this.userName = userName;
    this.fullName = fullName;
    this.email = email;
    this.phoneNumber = phoneNumber;
    this.roles = roles;
  }


  get friendlyName(): string {
    let name = this.fullName || this.userName;

    return name;
  }


  public id: string;
  public userName: string;
  public fullName: string;
  public email: string;
  public phoneNumber: string;
  public isEnabled: boolean;
  public isLockedOut: boolean;
  public roles: string[];
}
