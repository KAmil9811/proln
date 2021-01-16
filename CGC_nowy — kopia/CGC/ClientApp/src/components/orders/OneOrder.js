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
        return (
            <div>
                <Sidebar />
                <div className="nav_one_o">
                    
                </div>
                <div className="conteiner_one_o">
                    <div className="key">
                        <p>Id zlecenia: {sessionStorage.getItem('orderId')}</p>
                        <p>Zleceniodawca: {sessionStorage.getItem('klient')}</p>
                        <p>Deadline: {sessionStorage.getItem('deadline')}</p>
                        <p>Priorytet: {sessionStorage.getItem('priority')}</p>
                        <button type="button" className="orders3" onClick={this.dataEdit}>Edytuj dane</button>

                    </div>
                    <div className="oneordertable">
                        <OneOrderTable />
                    </div>
                </div>

            </div>
        );
    }
}