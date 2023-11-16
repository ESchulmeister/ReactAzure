/*eslint unicode-bom: ["error", "always"]*/
import React, {useState, useEffect } from 'react';

import ThumbnailCell from '../components/util/ThumbnailCell';

import DataGrid, {
    FilterRow,
    HeaderFilter,
    Column,
    Selection,
    Scrolling,
    Pager,
    Paging,
    SearchPanel

} from 'devextreme-react/data-grid';
import { azureService } from '../services/azure.service';

const   applyFilterTypes = [{
            key: 'auto',
            name: 'Immediately',
        }
];


    
export default function BlobList() {


     const [blobs, setBlobs] = useState([]);

    const currentFilter = applyFilterTypes[0].key;


    useEffect(() => {
        const fetchBlobs = async () => {
           const blobs = await azureService.listBlobs();
           setBlobs(blobs);
        };
        fetchBlobs();
    }, []);


 
   return (

            <div className="form">

                <h4 className="title action-link">Image List</h4>

                <div className="dx-fieldset">

                    <div className="loadCenter">
                        <DataGrid
                            dataSource={blobs}
                            hoverStateEnabled={true}
                            showBorders={true}
                            remoteOperations={true}
                            noDataText="No Images Available"
                            height="auto"
                            width="auto"
                            rowAlternationEnabled={true}
                        >

                       <SearchPanel visible={true} highlightCaseSensitive={true} />
                            <Scrolling rowRenderingMode='virtual'></Scrolling>
                            <FilterRow visible={true} applyFilter={currentFilter} />
                            <HeaderFilter visible={true} />
                            <Selection mode="single" />
                            <Paging defaultPageSize={3} />
                            <Pager
                                visible={true}
                                showInfo={true}
                                showNavigationButtons={true} />


                       <Column caption="#"
                            cellRender={ThumbnailCell}
                            alignment="center"
                            width="125"
                       />

                        <Column dataField="Name"   />
                        

                        </DataGrid>

                    </div>


                </div>

            </div>

       );
    };

