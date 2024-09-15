import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements AfterViewInit, OnInit {
  title?: string;
  titleService?: Title;

  constructor(titleService: Title, private route: ActivatedRoute, private router: Router) {
    this.titleService = titleService;
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

  ngAfterViewInit() {
    this.title = this.titleService?.getTitle();
  }
}