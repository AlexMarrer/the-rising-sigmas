import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  
  constructor(private titleService: Title, private translate: TranslateService) {
    this.titleService.getTitle();
    this.initializeTranslation();
  }

  initializeTranslation() {
    // default language
    this.translate.setDefaultLang('en');  

    const browserLang = this.translate.getBrowserLang();
    this.translate.use(browserLang.match(/en|de/) ? browserLang : 'en');
  }

  changeLanguage(language: string) {
    this.translate.use(language);
  }
  
}
