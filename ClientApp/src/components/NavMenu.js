import React, { Component } from 'react';
import { Collapse, Navbar, NavbarToggler, NavItem } from 'reactstrap';
import { Link } from 'react-router-dom';
import { authenticationService } from '../services/authentication.service.js';
import { Button } from 'devextreme-react/button';

//functional component - modal form
import { default as ProfileModal } from './ProfileModal.js';

import { getUser } from './util/Utils';

import './NavMenu.css';

//images
import imgLogout from '../images/logout.png';
import imgHome from '../images/Home.png';
import imgUpload from '../images/upload.png';
import imgList from '../images/imageList.png';


const logout =  async () => {
    authenticationService.logout();
};


export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);

        this.state = {
            collapsed: true,
            isLoggedIn: false,
        };

    }

    componentDidMount() {
        const user = getUser();
        if (user) {
            this.setState({ isLoggedIn: true });
        }
    }
   

   

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }




    render() {
        return (
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>

                    {this.state.isLoggedIn && (

                       

                        <ul className="navbar-nav small"   >

                            <li>

                                <NavItem to="/Home" tag={Link} >
                                    <img id="imgHome" src={imgHome}
                                        alt=""
                                        title="Home"
                                        className="mnuImg"
                                    ></img>
                                </NavItem>
                            </li>

                         
                            <li>

                                <NavItem to="/Upload" tag={Link} >
                                    <img id="imgUpload" src={imgUpload}
                                        alt=""
                                        title="Upload image"
                                        className="mnuImg"
                                    ></img>
                                </NavItem>
                            </li>

                            <li>

                                <NavItem to="/BlobList" tag={Link} >
                                    <img id="imgList" src={imgList}
                                        alt=""
                                        title="Image List"
                                        className="mnuImg"
                                    ></img>
                                </NavItem>
                            </li>


                            {/*profile - modal */}
                            <ProfileModal />

                            <li>
                                <Button icon={imgLogout} onClick={logout}
                                    hint="Log Out"
                                    className="mnuImg"
                                />
                            </li>

                        </ul>


                    )}
                </Collapse>
            </Navbar>
        );
    }
}
