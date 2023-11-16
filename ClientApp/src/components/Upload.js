/*eslint unicode-bom: ["error", "always"]*/
import React, { useState, useRef } from 'react';
import LoadIndicator from 'devextreme-react/load-indicator';
import { Button } from 'devextreme-react/button';
import { appError } from '../components/util/Utils';
import { azureService } from '../services/azure.service';
import authHeader from '../services/auth-header';

import errImg from '../images/error.png';
import imgWait from '../images/Wait.svg';

const fileExtensions =  ['.eps', '.svg', '.jpg', '.jpeg', '.gif', '.png', '.bmp'];
const MAX_FILE_SIZE = 4096 // 4MB



export default function Upload() {

    const [loading, setLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [isSelected, setIsSelected] = useState(false);
    const [currentFile, setCurrentFile] = useState(null);
    const [thumbUrl, setThumbUrl] = useState('');


    const selectFile = async (e) => {

        setIsSelected(false);

        setLoading(false);

        setErrorMessage('');

        setThumbUrl('');

        let file = e.target.files[0];

        setCurrentFile(file);

    };

 

    const uploadFile = async (e) => {


        e.event.preventDefault(); 

        setErrorMessage('');

        if (!currentFile) {
            return;
        }

        const sizeBytes = currentFile.size / 1024;

        if (sizeBytes > MAX_FILE_SIZE) {
            setErrorMessage("File size is greater than maximum limit - 4MB");
            return;
        }


        try {

            setLoading(true); 

            let result = await azureService.uploadFile(currentFile);
              
            if (!result.ok) {
                setLoading(false);
                setThumbUrl('');
                setErrorMessage(appError);
                return;
            }

            setLoading(false);

            setIsSelected(true);

            let response = await result.json();

            console.log('res', response);

            if (response.imageMetadata) {
                setThumbUrl(response.imageMetadata.Uri);
            }

        }

        catch(ex)
        {
            console.log('Error', ex);

            setIsSelected(false);

            setErrorMessage(appError);

            setCurrentFile(null);
        }
    
    };

   
    return (
        <>

            <form encType="multipart/form-data" >

           <h4 className="title action-link">Upload File(s) To Azure</h4>       

            {errorMessage && (
                <table className="alert">
                    <tbody>
                        <tr>
                            <td>
                                <img src={errImg} alt="" className="errImg"></img>
                            </td>
                            <td>
                                {errorMessage}
                            </td>
                            <td>
                                <img src={errImg} alt="" className="errImg"></img>
                            </td>
                        </tr>
                    </tbody>
                </table>
            )}


            <span className="preview">
                {
                    loading ?
                        <LoadIndicator indicatorSrc={imgWait} height={65} width={65} visible={true} />
                        : ''
                }
            </span>

            <table>

                <tbody>

                    <tr>
                        <td colSpan="3" >
                            Allowed file extensions : <b>.eps, .svg, .jpeg, .gif, .png, .bmp</b>
                        </td>

                    </tr>
                    <tr>
                            <td colSpan="3">
                            Less than 4 MB. Dimensions should be greater than 50 x 50.
                    </td>
                    </tr>

                    <tr>
                        <td colSpan="3">&nbsp;</td>
                    </tr>

                    <tr>


                        {/*<td style={{'width':'20%'} }>*/}
                        {/*   <label  className="oLabel"> Name</label>*/}
                        {/*</td>*/}

                        {/*<td>*/}

                        {/*        <TextBox name="txtName"*/}
                        {/*            placeholder="Name"*/}
                        {/*            value={Name }*/}
                        {/*            className="dxControl"*/}
                        {/*            maxLength={255}*/}
                        {/*            ref={textBox}*/}
                        {/*            onValueChanged={(e) => setName(e.value)}*/}
                        {/*    </TextBox>*/}
                            {/*</td>*/}

                            <td>
                                <label className="custom-file-upload" >Select Image
                                        <input
                                            id="fileUpload"
                                            name="fileUpload"
                                            type="file"
                                            onChange={selectFile}
                                        accept={fileExtensions}
                                        />
                                </label>
                            </td>
                    
                            <td align="center" >

                                <div>
                                    {currentFile && (
                                        <div className="custLabel">&nbsp; {(currentFile) ? currentFile.name.split('.').slice(0, -1).join('.') : ''}</div>

                                    ) }
                                </div>
                            </td>
                 

                            <td  align="center">
                                <Button
                                    text="Upload Image"
                                    useSubmitBehavior={false}
                                    onClick={(e) =>uploadFile(e)}
                                    disabled={!currentFile}
                                    icon="upload"
                                >


                            </Button>
                  
                        </td>
                        </tr>

                </tbody>


                </table>

                <hr />


                {
                    (!errorMessage) && isSelected ? (
                        <div className="preview">
                            <p className="oLabel "><span className="tagLabel">{currentFile.name.split('.').slice(0, -1).join('.')}</span> successfully uploaded</p>
                        </div>
                    ) : (
                        ''
              )}

            
                {thumbUrl && (
                    <div className="preview">
                        <img  src={thumbUrl} alt="" width="180" height="180" />
                    </div>
                )}


            </form>

        </>
    );
};
