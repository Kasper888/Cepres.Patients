import { Component, Injector, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { forEach as _forEach, includes as _includes, map as _map } from 'lodash-es';
import { AppComponentBase } from '@shared/app-component-base';
import { ReportingServiceProxy, PatientStatistics } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './patient-report.component.html'
})
export class PatientReportComponent extends AppComponentBase
    implements OnInit {
    id: number;

    public patientReport: PatientStatistics = new PatientStatistics();
    constructor(
        injector: Injector,
        public _reportingService: ReportingServiceProxy,
        public bsModalRef: BsModalRef
    ) { super(injector); }
    ngOnInit(): void {
        this.bindPatientReport(this.id);
    }

    bindPatientReport(id: number) {
        this.id = id;
        this._reportingService.getPatientStatistics(this.id)
            .subscribe((result: PatientStatistics) => {
                this.patientReport = result;
            });
    }
}
