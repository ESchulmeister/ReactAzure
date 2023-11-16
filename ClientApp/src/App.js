import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { Header } from './components/Header';
import { Footer } from './components/Footer';

import { default as PageNotFound } from './components/util/PageNotFound';


import AuthVerify from './services/auth-verify';   ///component validates user token


import 'devextreme/dist/css/dx.light.css';
import 'devextreme/dist/css/dx.common.css';
import './custom.css';


export default class App extends Component {
  static displayName = App.name;

  render() {
      return (
        <div>
            <AuthVerify /> 
             <Header />
                <Layout>
                  <Routes>
                      <Route path="*" element={<PageNotFound />} />

                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;                      


                            return <Route key={index} {...rest} element={element} />;
                        })}
                    </Routes>
              </Layout>

             <Footer />
        </div>
    );
  }
}
