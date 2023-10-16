import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { tap } from 'rxjs/operators';
import { chatMesage } from './chatMesage';
@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private hubConnection!: HubConnection;
  public messages: BehaviorSubject<any> = new BehaviorSubject<any>([]);
  private connectionUrl = 'https://localhost:7009/hub';
  // private apiUrl = 'https://localhost:44319/api/chat';

  constructor(private http: HttpClient) {}

  // public sendMessageToApi(message: string) {
  //   return this.http
  //     .post(this.apiUrl, this.buildChatMessage(message))
  //     .pipe(
  //       tap((_) => console.log('message sucessfully sent to api controller'))
  //     );
  // }

  public sendMessageToHub(message: string) {
    var promise = this.hubConnection
      .invoke('BroadcastAsync', this.buildChatMessage(message))
      .then(() => {
        console.log('message sent successfully to hub');
      })
      .catch((err) =>
        console.log('error while sending a message to hub: ' + err)
      );

    return from(promise);
  }

  private getConnection(): HubConnection {
    return (
      new HubConnectionBuilder()
        .withUrl(this.connectionUrl)
        // .withHubProtocol(new MessagePackHubProtocol())
        //  .configureLogging(LogLevel.Trace)
        .build()
    );
  }

  private buildChatMessage(message: string): chatMesage {
    return {
      ConnectionId: this.hubConnection.connectionId,
      Text: message,
      DateTime: new Date(),
    };
  }

  private addListeners() {
    // this.hubConnection.on('messageReceivedFromApi', (data: chatMesage) => {
    //   console.log('message received from API Controller');
    //   let newValue = this.messages.value;
    //   newValue.push(data);
    //   this.messages.next(newValue);
    // });
    this.hubConnection.on('newTask', (data: any) => {
      console.log('message received from Hub');
      console.log(data);
      // let newValue = this.messages.value;
      // newValue.push(data);
      this.messages.next(data);
    });
    // this.hubConnection.on('newUserConnected', (_) => {
    //   console.log('new user connected');
    // });
  }
  listenTasks(correlationId: string): void {
    this.hubConnection = this.getConnection();

    this.hubConnection
      .start()
      .then(() => {
        console.log('connection started');
        this.hubConnection.invoke('join', correlationId);
        this.addListeners();
      })
      .catch((err) =>
        console.log('error while establishing signalr connection: ' + err)
      );
  }
}
