import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NewsService, Article } from './news.service';

@Component({
  selector: 'app-news-detail',
  template: `
  <div *ngIf="item; else loading">
    <h2>{{item.headLine}}</h2>
    <div class="meta">{{item.createdDate | date:'medium'}}</div>
    <p>{{item.content}}</p>
    <p><a routerLink="/">Back to list</a></p>
  </div>
  <ng-template #loading>
    <p>Loading...</p>
  </ng-template>
  `
})
export class NewsDetailComponent implements OnInit {
  item?: Article;
  constructor(private route: ActivatedRoute, private svc: NewsService) { }
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.svc.get(id).subscribe(r => this.item = r);
  }
}
