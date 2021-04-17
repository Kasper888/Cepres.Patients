import { Component, Injector, ViewChild, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { DataManager, ODataV4Adaptor, Query } from '@syncfusion/ej2-data';
import { AppConsts } from '@shared/AppConsts';
import { AddEventArgs, CommandModel, GridComponent, GridModel } from '@syncfusion/ej2-angular-grids';
import { BsModalService } from 'ngx-bootstrap/modal';
import { PatientReportComponent } from './patient-report.component';

@Component({
  templateUrl: './patients.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientsComponent extends AppComponentBase {
  @ViewChild('patientGrid') patientGrid: GridComponent;

  public patientVisitsGrid: GridModel | GridComponent;
  public patientData: DataManager;
  public patientVisitsData: DataManager;
  public commands: CommandModel[];

  constructor(injector: Injector,
    private _modalService: BsModalService) {
    super(injector);
  }
  ngOnInit(): void {
    this.patientData = this.createODataManager("Patient");
    this.patientVisitsData = this.createODataManager("Visit");
    this.commands = [{ buttonOption: { iconCss: 'fas fa-address-card', content: ' ' } }];
    this.patientVisitsGrid = {
      dataSource: this.patientVisitsData,
      queryString: 'PatientId',
      clipMode: 'EllipsisWithTooltip',
      toolbar: ['Add', 'Edit', 'Cancel', 'Update'],
      filterSettings: { type: "Menu" },
      sortSettings: { columns: [{ field: "CreationTime", direction: "Descending" }] },
      editSettings: { allowEditing: true, allowAdding: true, newRowPosition: "Top" },
      allowExcelExport: false, allowFiltering: true, allowPaging: true, allowReordering: true, allowResizing: true, allowSorting: true, allowSelection: true, showColumnChooser: true,
      columns: [
        { field: 'Id', isIdentity: true, isPrimaryKey: true, defaultValue: '0', width: '10%' },
        { field: 'CreationTime', headerText: 'Visit On', width: '20%', format: 'y-M-d h:mm a', editType: 'datetimepickeredit' },
        { field: 'Disease', editType: 'dropdownedit', width: '20%' },
        { field: 'Fees', format: 'C2', editType: 'numericedit', width: '10%', validationRules: { required: true, range: [1, 99999] } },
        { field: 'Description', width: '40%', validationRules: { maxLength: 800 } }
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
  private createODataManager(entity: String) {
    return new DataManager({
      crossDomain: true,
      url: AppConsts.remoteServiceBaseUrl + '/odata/' + entity,
      adaptor: new ODataV4Adaptor(),
      headers: [{ 'Authorization': 'Bearer ' + abp.auth.getToken() }]
    });
  }
  onActionFailure(e) {
    this.notify.error("National Id is already taken!", 'Error');
  }
  commandClick(args: any): void {
    this._modalService.show(PatientReportComponent,
      {
        class: 'modal-lg',
        initialState: {
          id: args.rowData.Id,
        },
      }
    );

  }
}
