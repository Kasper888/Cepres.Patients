import { PatientsTemplatePage } from './app.po';

describe('Patients App', function() {
  let page: PatientsTemplatePage;

  beforeEach(() => {
    page = new PatientsTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
