import { Component, Injector, ViewChild, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { DataManager, ODataV4Adaptor } from '@syncfusion/ej2-data';
import { AppConsts } from '@shared/AppConsts';
import { CommandClickEventArgs, CommandModel, GridComponent } from '@syncfusion/ej2-angular-grids';
@Component({
  templateUrl: './patients.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientsComponent extends AppComponentBase {
  @ViewChild('grid')
  public grid: GridComponent;
  public data: DataManager;
  public commands: CommandModel[];
  constructor(injector: Injector) {
    super(injector);
  }
  ngOnInit(): void {
    this.commands = [{ buttonOption: { iconCss: 'fas fa-address-card', content: ' ' } }];

    this.data = new DataManager({
      crossDomain: true,
      url: AppConsts.remoteServiceBaseUrl + '/odata/Patient?$expand=Visits',
      adaptor: new ODataV4Adaptor(),
      headers: [{ 'Authorization': 'Bearer ' + abp.auth.getToken() }]
    });
  }
  onActionFailure(e) {
    this.notify.error(e.error[0].error.status + ' | ' + e.error[0].error.statusText);
  }
  toolbarClick(args): void {
    switch (args.item.text) {
      case 'Excel Export':
        this.grid.excelExport();
        break;
    }
  }
  commandClick(args: CommandClickEventArgs): void {
    alert(JSON.stringify(args.rowData));
  }
}
