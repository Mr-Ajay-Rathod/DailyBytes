import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { NewsListComponent } from './news-list.component';
import { NewsDetailComponent } from './news-detail.component';

const routes: Routes = [
  { path: '', component: NewsListComponent },
  { path: 'news/:id', component: NewsDetailComponent }
];

@NgModule({
  declarations: [AppComponent, NewsListComponent, NewsDetailComponent],
  imports: [BrowserModule, HttpClientModule, RouterModule.forRoot(routes)],
  bootstrap: [AppComponent]
})
export class AppModule { }
