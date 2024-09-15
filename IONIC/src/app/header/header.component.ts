import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Route, NavigationEnd } from '@angular/router';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent  implements OnInit {

  title?: string;
  titleService?: Title;

  constructor(titleService: Title, private activatedRoute: ActivatedRoute, private route: Route) {
    this.titleService = titleService;
   }

  ngOnInit() {
    // this.title = this.titleService.getTitle();
  }

}
