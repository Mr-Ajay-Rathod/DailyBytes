import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
  <div class="container">
    <h1>DailyBytes</h1>
    <router-outlet></router-outlet>
  </div>
  `,
  styles: [`.container { max-width: 900px; margin: 20px auto; font-family: Arial, Helvetica, sans-serif; }`]
})
export class AppComponent { }
