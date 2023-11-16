/*eslint unicode-bom: ["error", "always"]*/

import React, { useState } from 'react';
import { Modal } from 'react-bootstrap';
import { getUser } from './util/Utils'; 
import { Button } from 'devextreme-react/button';

//image
import imgProfile from '../images/profile.png';

export default function ProfileModal() {
    const [show, setShow] = useState(false);


    //evt methods
    const onClose = () => setShow(false);
    const onShow = () => setShow(true);

    const user = getUser();

    if (user === undefined || user == null) {    //nothing to render; 
        return null;
    }

    return (
        
            <>
            <Button icon={imgProfile} 
                    onClick = {onShow}
                    hint="Current User Profile"
                    className="mnuImg"
            />


            <Modal show={show} size="sm" onHide={onClose}>
                <Modal.Header  >
                    <Modal.Title className="title">Profile</Modal.Title>
                    <Button variant="secondary" onClick={onClose} hint="Close">x</Button>
                </Modal.Header>
                <Modal.Body>
                    <div className="dx-field" >
                        <div className="dx-field-label  dxLabel">Name</div>
                        <div className="dx-field-value">{user.FullName}</div>
                    </div>               
                    <div className="dx-field"  >
                        <div className="dx-field-label  dxLabel">Login</div>
                        <div className="dx-field-value">{user.Login}</div>
                    </div>
                    <div className="dx-field"  >
                        <div className="dx-field-label  dxLabel">Role</div>
                        <div className="dx-field-value">{user.Role.Name}</div>
                    </div>
                    <div className="dx-field"  >
                        <div className="dx-field-label  dxLabel">Clock</div>
                        <div className="dx-field-value">{user.Clock}</div>
                    </div>
                    <div className="dx-field"  >
                        <div className="dx-field-label  dxLabel">FTE</div>
                        <div className="dx-field-value">{user.FTE}</div>
                    </div>
                </Modal.Body>              

            </Modal>
        </>

    );
}