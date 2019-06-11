// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

import { AppPage } from './app.po';

describe('Biblifun.Website App', () => {
    let page: AppPage;

    beforeEach(() => {
        page = new AppPage();
    });

    it('should display application title: Biblifun.Website', () => {
        page.navigateTo();
        expect(page.getAppTitle()).toEqual('Biblifun.Website');
    });
});
