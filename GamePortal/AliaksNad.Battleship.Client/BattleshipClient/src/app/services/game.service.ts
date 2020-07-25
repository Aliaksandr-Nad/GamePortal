import { BattleAreaDto } from './../models/BattleAreaDto';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { share } from 'rxjs/operators';

@Injectable({ providedIn: 'root'})
export class GameService {

  serviceId: string;

  constructor(private http: HttpClient) {
    this.serviceId = Date.now().toString();
   }

   getById(id: number){
      return this.http
      .get<BattleAreaDto>(`https://localhost:44313/api/battleship/game/${id}`)
      .pipe(share());
   }

   getAll(){
    return this.http
    .get<BattleAreaDto[]>(`https://localhost:44313/api/battleship/game`)
    .pipe(share());
 }
}