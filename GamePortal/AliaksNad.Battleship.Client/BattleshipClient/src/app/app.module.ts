import { BattleshipRoutesModule } from './battleshiproutes/battleshiproutes.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/main/navbar/navbar.component';
import { LoginComponent } from './components/security/login/login.component';
import { AreaComponent } from './components/game/area/area.component';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './components/main/home/home.component';
import { NotFoundComponent } from './components/main/notfound/notfound.component';
import { GameBoardComponent } from './components/game/game-board/game-board.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    AreaComponent,
    HomeComponent,
    NotFoundComponent,
    GameBoardComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BattleshipRoutesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
