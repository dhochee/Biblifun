// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { AppTranslationService } from './app-translation.service';
import { ThemeManager } from './theme-manager';
import { LocalStoreManager } from './local-store-manager.service';
import { DBkeys } from './db-Keys';
import { Utilities } from './utilities';
import { environment } from '../../environments/environment';

interface UserConfiguration {
  language: string;
  homeUrl: string;
  themeId: number;
}

@Injectable()
export class ConfigurationService {

  constructor(
    private localStorage: LocalStoreManager,
    private translationService: AppTranslationService,
    private themeManager: ThemeManager) {

    this.loadLocalChanges();
  }

  set language(value: string) {
    this._language = value;
    this.saveToLocalStore(value, DBkeys.LANGUAGE);
    this.translationService.changeLanguage(value);
  }
  get language() {
    return this._language || ConfigurationService.defaultLanguage;
  }


  set themeId(value: number) {
    value = +value;
    this._themeId = value;
    this.saveToLocalStore(value, DBkeys.THEME_ID);
    this.themeManager.installTheme(this.themeManager.getThemeByID(value));
  }
  get themeId() {
    return this._themeId || ConfigurationService.defaultThemeId;
  }


  set homeUrl(value: string) {
    this._homeUrl = value;
    this.saveToLocalStore(value, DBkeys.HOME_URL);
  }
  get homeUrl() {
    return this._homeUrl || ConfigurationService.defaultHomeUrl;
  }
    
  public static readonly appVersion: string = '2.7.2';

  // ***Specify default configurations here***
  public static readonly defaultLanguage: string = 'en';
  public static readonly defaultHomeUrl: string = '/';
  public static readonly defaultThemeId: number = 1;

  public baseUrl = environment.baseUrl || Utilities.baseUrl();
  public tokenUrl = environment.tokenUrl || environment.baseUrl || Utilities.baseUrl();
  public loginUrl = environment.loginUrl;
  public fallbackBaseUrl = 'http://quickapp.ebenmonney.com';
  // ***End of defaults***

  private _language: string = null;
  private _homeUrl: string = null;
  private _themeId: number = null;

  private onConfigurationImported: Subject<boolean> = new Subject<boolean>();
  configurationImported$ = this.onConfigurationImported.asObservable();



  private loadLocalChanges() {

    if (this.localStorage.exists(DBkeys.LANGUAGE)) {
      this._language = this.localStorage.getDataObject<string>(DBkeys.LANGUAGE);
      this.translationService.changeLanguage(this._language);
    } else {
      this.resetLanguage();
    }


    if (this.localStorage.exists(DBkeys.THEME_ID)) {
      this._themeId = this.localStorage.getDataObject<number>(DBkeys.THEME_ID);
      this.themeManager.installTheme(this.themeManager.getThemeByID(this._themeId));
    } else {
      this.resetTheme();
    }


    if (this.localStorage.exists(DBkeys.HOME_URL)) {
      this._homeUrl = this.localStorage.getDataObject<string>(DBkeys.HOME_URL);
    }
  }


  private saveToLocalStore(data: any, key: string) {
    setTimeout(() => this.localStorage.savePermanentData(data, key));
  }


  public import(jsonValue: string) {

    this.clearLocalChanges();

    if (jsonValue) {
      const importValue: UserConfiguration = Utilities.JsonTryParse(jsonValue);

      if (importValue.language != null) {
        this.language = importValue.language;
      }

      if (importValue.themeId != null) {
        this.themeId = +importValue.themeId;
      }

      if (importValue.homeUrl != null) {
        this.homeUrl = importValue.homeUrl;
      }
    }

    this.onConfigurationImported.next();
  }


  public export(changesOnly = true): string {

    const exportValue: UserConfiguration = {
      language: changesOnly ? this._language : this.language,
      themeId: changesOnly ? this._themeId : this.themeId,
      homeUrl: changesOnly ? this._homeUrl : this.homeUrl,
    };

    return JSON.stringify(exportValue);
  }


  public clearLocalChanges() {
    this._language = null;
    this._themeId = null;
    this._homeUrl = null;

    this.localStorage.deleteData(DBkeys.LANGUAGE);
    this.localStorage.deleteData(DBkeys.THEME_ID);
    this.localStorage.deleteData(DBkeys.HOME_URL);

    this.resetLanguage();
    this.resetTheme();
  }


  private resetLanguage() {
    const language = this.translationService.useBrowserLanguage();

    if (language) {
      this._language = language;
    } else {
      this._language = this.translationService.useDefaultLangage();
    }
  }

  private resetTheme() {
    this.themeManager.installTheme();
    this._themeId = null;
  }
}
