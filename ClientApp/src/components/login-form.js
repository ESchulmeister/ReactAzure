/*eslint unicode-bom: ["error", "always"]*/
import React, { useState , useRef, useCallback} from 'react';
import LoadIndicator from 'devextreme-react/load-indicator';
import { TextBox, Button as TextBoxButton } from 'devextreme-react/text-box';
import { Button } from 'devextreme-react/button';
import {
    Validator,
    RequiredRule,
} from 'devextreme-react/validator';

import { authenticationService } from '../services/authentication.service.js';

import { appError } from '../components/util/Utils';


import errImg from '../images/error.png';
import loadImg from '../images/spinner.svg';

export default function Connect() {

    //state
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [isEnabled, setEnabled] = useState(true);
    const [loading, setLoading] = useState(false);
    const [passwordMode, setPasswordMode] = useState('password');
    const [errorMessage, setErrorMessage] = useState('');

    const textBox = useRef(null);

    const handleKeyPress = useCallback((e) => {

        setErrorMessage('');

    }, []);

    const focusTextBox = useCallback(() => {
     
        textBox.current.instance.focus();
    }, []);
    
    const passwordButton = {
        type: 'default',
        onClick: () => {
            const newMode = passwordMode === 'text' ? 'password' : 'text';
            setPasswordMode(newMode);
        },
    };

    const onSubmit = async (e, userName, password) => {


        e.event.preventDefault();

        let result = e.validationGroup.validate();  //validate input

        if (!result.isValid) {
            setErrorMessage('');
            setEnabled(true);
            return;
        }

        setErrorMessage('');
        setEnabled(false);     //disable login
        setLoading(true);       //show loading image

        let status = null;



        try {

            status = await authenticationService.login(userName, password);

            setEnabled(true);
            setLoading(false);


            switch (status) {
                case 200: {
                    window.location.href = window.location.origin + '/Home';
                    break;
                }
                case 400: {
                    setErrorMessage('Credentials not detected');
                    focusTextBox();
                    break;
                }
                case 404: {
                    setErrorMessage('Unknown username and/or password');
                    focusTextBox();
                    break;
                }
                case 424: {
                    setErrorMessage('User not found in the database');
                    focusTextBox();
                    break;
                }
                default: {
                    setErrorMessage(appError);
                    focusTextBox();
                    break;
                }
            }
        }
        catch (e) {
            setErrorMessage(appError);

            console.error('Error', e.message);
        }
    };


    return (

        <form  method='post'>

            <h4 className="title action-link">Sign In</h4>            

            <span className="loadCenter">
            {
                loading ?
                <LoadIndicator indicatorSrc={loadImg} height={55} width={55} visible={true} />
                        : ''
        
            }
        </span>
         
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
  

                <div className="dx-fieldset" id="container">  

                    <div className="dx-field">
                        <div className="dx-field-label tagLabel">UserName</div>
                        <div className="dx-field-value" >
                            <TextBox name="username"
                                value={username}
                                placeholder="UserName"
                                className="dxControl"
                                maxLength={255}
                                onKeyPress={handleKeyPress}
                                ref={textBox} 
                                onValueChanged={(e) => setUsername(e.value)}
                            >
                                <Validator>
                                    <RequiredRule  />
                                </Validator>
                            </TextBox>

                        </div>
                </div>

              


            <div className="dx-field">
                    <div className="dx-field-label tagLabel">Password</div>
                    <div className="dx-field-value" >

                        <TextBox name="password"
                            className="dxControl"
                            value={password}
                            maxLength={8}
                            onValueChanged={(e) => setPassword(e.value)}
                            mode={passwordMode}
                            onKeyPress={handleKeyPress}
                            placeholder="Password"
                            showClearButton={true}
                        >
                            <TextBoxButton
                                name="eyeButton"
                                location="after"
                                options={passwordButton}
                            />                          
                            <Validator>
                            <RequiredRule  />
                            </Validator>
                        </TextBox>
                    </div>

            </div>



            <div className="dx-field">
                <Button
                    text="Log In"
                    className="button btn-primary"
                    width={120}
                    useSubmitBehavior={false}
                    onClick={(e) => onSubmit(e, username, password)}
                    disabled={!isEnabled}
                >


                </Button>

            </div>

        </div>
    </form>

        )
    };
