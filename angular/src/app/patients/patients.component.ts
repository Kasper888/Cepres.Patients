import { Component, Injector, ViewChild, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { DataManager, ODataV4Adaptor, Query } from '@syncfusion/ej2-data';
import { AppConsts } from '@shared/AppConsts';
import { AddEventArgs, CommandClickEventArgs, CommandModel, GridComponent, GridModel } from '@syncfusion/ej2-angular-grids';
import { Router } from '@angular/router';
@Component({
  templateUrl: './patients.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientsComponent extends AppComponentBase {
  @ViewChild('patientGrid')
  public patientGrid: GridComponent;
  public patientVisitsGrid: GridModel | GridComponent;
  public patientData: DataManager;
  public patientVisitsData: DataManager;

  public query: Query;
  public commands: CommandModel[];
  constructor(injector: Injector, private router: Router) {
    super(injector);
  }
  ngOnInit(): void {
    this.patientData = new DataManager({
      crossDomain: true,
      url: AppConsts.remoteServiceBaseUrl + '/odata/Patient',
      adaptor: new ODataV4Adaptor(),
      headers: [{ 'Authorization': 'Bearer ' + abp.auth.getToken() }]
    });
    this.patientVisitsData = new DataManager({
      crossDomain: true,
      url: AppConsts.remoteServiceBaseUrl + '/odata/Visit',
      adaptor: new ODataV4Adaptor(),
      headers: [{ 'Authorization': 'Bearer ' + abp.auth.getToken() }]
    });
    this.commands = [{ buttonOption: { iconCss: 'fas fa-address-card', content: ' ' } }];
    this.patientVisitsGrid = {
      dataSource: this.patientVisitsData,
      queryString: 'PatientId',
      clipMode: 'EllipsisWithTooltip',
      toolbar: ['Add', 'Edit', 'Cancel', 'Update'],
      filterSettings: { type: "Menu" },
      sortSettings: { columns: [{ field: "CreationTime", direction: "Descending" }] },
      editSettings: { allowEditing: true, allowAdding: true, newRowPosition: "Top" },
      allowFiltering: true, allowReordering: true, allowResizing: true, allowSorting: true, allowSelection: true, showColumnChooser: true,
      columns: [
        { field: 'Id', isIdentity: true, isPrimaryKey: true, defaultValue: '0', width: '10%' },
        { field: 'CreationTime', headerText: 'Visit On', width: '20%', format: 'y-M-d h:mm a', editType: 'datetimepickeredit' },
        { field: 'Disease', editType: 'dropdownedit', width: '20%' },
        { field: 'Fees', format: 'C2', editType: 'numericedit', width: '10%', validationRules: { required: true, range: [1, 99999] } },
        { field: 'Description', width: '40%', validationRules: { maxLength: 800 }  }
      ],
      actionBegin(args: AddEventArgs) {
        if (args.requestType === 'add') {
          const PatientId = 'PatientId';
          (args.data as object)[PatientId] = this.parentDetails.parentKeyFieldValue;
        }
      },
      load() {
        const PatientId = 'Id';
        (this as GridComponent).parentDetails.parentKeyFieldValue = (<{ PatientId?: string }>(this as GridComponent).parentDetails.parentRowData)[PatientId];
      }
    };
  }
  onActionFailure(e) {
    this.notify.error("National Id is already taken!", 'Error');
  }
  toolbarClick(args): void {
    switch (args.item.text) {
      case 'Excel Export':
        this.patientGrid.excelExport();
        break;
    }
  }
  commandClick(args: any): void {
    this.router.navigateByUrl('/PatientReport/' + args.rowData.Id);
  }
}
