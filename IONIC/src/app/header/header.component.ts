import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements AfterViewInit, OnInit {
  title?: string;
  titleService?: Title;
  translate?: TranslateService;

  constructor(titleService: Title, private route: ActivatedRoute, private router: Router, translateService: TranslateService) {
    this.titleService = titleService;
    this.translate = translateService;
  }

  ngOnInit() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => {
        let child = this.route.firstChild;
        while (child?.firstChild) {
          child = child.firstChild;
        }
        return child?.snapshot.data['title'] || this.titleService?.getTitle();
      })
    ).subscribe((title: string) => {
      this.title = title;
      this.titleService?.setTitle(title);
    });
  }

  changeLanguage(language: string) {
    this.translate.use(language);
    localStorage.setItem('language', language);
  }

  ngAfterViewInit() {
  }
}