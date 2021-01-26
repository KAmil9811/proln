import React, { Component } from 'react';
import { OneOrderTable } from './OneOrderTable'
import './OneOrder.css'
import Sidebar from '../Sidebar';

export class OneOrder extends Component {
    displayName = OneOrder.name;
    constructor(props) {
        super(props);
    }

    homePage = (event) => {
        this.props.history.push('/home');
        sessionStorage.removeItem('orderId');
        sessionStorage.removeItem('klient');
        sessionStorage.removeItem('deadline');
        sessionStorage.removeItem('priority');
    }

    orders = (event) => {
        this.props.history.push('/orderwarehouse');
        sessionStorage.removeItem('orderId');
        sessionStorage.removeItem('klient');
        sessionStorage.removeItem('deadline');
        sessionStorage.removeItem('priority');
    }

    dataEdit = (event) => {
        //id,klient i deadline są już w pamięci
        this.props.history.push('/edit_order');

    }


    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('orderManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div>
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Order</h1>
                    </div>

                    <div className="one_order_conteiner">
                        <div className="key">
                            <p>Id zlecenia: {sessionStorage.getItem('orderId')}</p>
                            <p>Zleceniodawca: {sessionStorage.getItem('klient')}</p>
                            <p>Deadline: {sessionStorage.getItem('deadline')}</p>
                            <p>Priorytet: {sessionStorage.getItem('priority')}</p>

                        </div>
                        <button type="button" className="prim_one_order" onClick={this.dataEdit}>Edit order</button>

                        <div className="oneordertable">
                            <OneOrderTable />
                        </div>
                    </div>

                </div>
            );
        }
        else {
            return (
                <div>
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Order</h1>
                    </div>

                    <div className="one_order_conteiner">
                        <div className="key">
                            <p>Id zlecenia: {sessionStorage.getItem('orderId')}</p>
                            <p>Zleceniodawca: {sessionStorage.getItem('klient')}</p>
                            <p>Deadline: {sessionStorage.getItem('deadline')}</p>
                            <p>Priorytet: {sessionStorage.getItem('priority')}</p>

                        </div>

                        <div className="oneordertable">
                            <OneOrderTable />
                        </div>
                    </div>

                </div>
            );
        }
    }
}