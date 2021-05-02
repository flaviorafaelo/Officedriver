import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Action } from 'rxjs/internal/scheduler/Action';
import { RouteService } from 'src/app/services/route.service';

@Component({
  selector: 'app-data-edit',
  templateUrl: './data-edit.component.html',
  styleUrls: ['./data-edit.component.less']
})
export class DataEditComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router, private routeService: RouteService) { }

  ngOnInit(): void {

    this.routeService.getActionsByUrl(this.router.url).subscribe(actions=>{
      console.log(actions);
    })

    this.route.queryParams.subscribe(params => {
  //    this.name = params['name'];
    });

   // RouteService
  }

}
