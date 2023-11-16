
/*eslint unicode-bom: ["error", "always"]*/

import React, { Component } from 'react';

import DataGrid, {
    Column,
    Selection,
    FilterRow,
    HeaderFilter,
    Paging,
    Pager, 
} from 'devextreme-react/data-grid';

import authHeader from '../services/auth-header';
import ThumbnailCell from '../components/util/ThumbnailCell';

//import { azureService } from '../services/azure.service';
import { createStore } from 'devextreme-aspnet-data-nojquery';

const dataSource = new createStore({
    key: 'ID',
    loadUrl: `/azure/ListBlobs`, 

    onBeforeSend: (method, ajaxOptions) => {
        ajaxOptions.xhrFields = { withCredentials: true };
        ajaxOptions.headers = authHeader();
    },
})


export class DxQuestion extends Component {
    static displayName = DxQuestion.name;

    constructor(props) {
        super(props);

        this.applyFilterTypes = [{
            key: 'auto',
            name: 'Immediately',
        }];

        this.state = {
            showFilterRow: true,
            showHeaderFilter: true,
            currentFilter: this.applyFilterTypes[0].key,
        };

        this.dataGridRef = React.createRef();

     
    }

 
    get dataGrid() {
        return this.dataGridRef.current.instance;
    }


    render() {
        return (
            <div className="form">

                <h2 className="title action-link">Image List</h2>

                <div className="dx-fieldset">

                    <div className="dx-field">

                        <DataGrid
                            showBorders={true}
                            dataSource={dataSource}
                            ref={this.dataGridRef}
                            remoteOperations={true}
                        >
                            <FilterRow visible={true} applyFilter={this.state.currentFilter} />
                            <HeaderFilter visible={true} />
                            <Selection mode="single" />
                            <Pager
                                visible={true}
                                showInfo={true}
                                showNavigationButtons={true} />
                            <Paging defaultPageSize={3} />


                            <Column caption="#"
                                cellRender={ThumbnailCell}
                                alignment="center"
                                width="auto"
                                allowFiltering={false}
                                allowSorting={false} 
                            />

                            <Column dataField="Name"       />

                        </DataGrid>
                    </div>
                </div>

            </div>

        );
    }

}