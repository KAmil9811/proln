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
                    <h1>Zaloguj się, aby usyskać dostęp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (
                <div>
                    <Sidebar />

                    <div className="one_order_conteiner">
                        <div className="key">
                            <p>Id zlecenia: {sessionStorage.getItem('orderId')}</p>
                            <p>Zleceniodawca: {sessionStorage.getItem('klient')}</p>
                            <p>Deadline: {sessionStorage.getItem('deadline')}</p>
                            <p>Priorytet: {sessionStorage.getItem('priority')}</p>

                        </div>
                        <button type="button" className="prim_one_order" onClick={this.dataEdit}>Edytuj dane</button>

                        <div className="oneordertable">
                            <OneOrderTable />
                        </div>
                    </div>

                </div>
            );
        }
    }
}