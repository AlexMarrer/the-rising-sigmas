import { Component, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  @Input() title?: string;
  translate?: TranslateService;

  constructor(translateService: TranslateService) {
    this.translate = translateService;
    this.translate.setDefaultLang('en');

    this.initializeTranslation();
  }

  initializeTranslation() {
    const browserLang = this.translate.getBrowserLang();
    const langToStore = browserLang.match(/en|de/) ? browserLang : 'en';
    localStorage.setItem('language', langToStore);
    this.translate.use(langToStore);
  }

  changeLanguage(language: any) {
    if (!language) {
      return;
    }
    if (!(language instanceof String)) {
      language = language.detail.value;
    }
    language = language.toLowerCase();
    this.translate.use(language);
    localStorage.setItem('language', language);
  }
}
