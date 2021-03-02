import React, { Component } from "react";
import './EditOrder.css';
import Sidebar from '../Sidebar';

export class EditOrder extends Component {
    displayName = EditOrder.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }

    handleEdit = (event) => {
        event.preventDefault();
        const receiver = {
            order: {
                deadline: this.refs.deadline.value,
                owner: this.refs.klient.value,
                priority: sessionStorage.getItem('priority'),
                id_order: sessionStorage.getItem('orderId'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            }
        }

        
    }


    handleEditOrder = (event) => {
        event.preventDefault();
        const receiver = {
            order: {
                deadline: this.refs.deadline.value,
                owner: this.refs.klient.value,
                priority: this.refs.priority.value,
                id_order: sessionStorage.getItem('orderId'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            }
        }

        if (this.refs.deadline.value === '') {
            alert("Enter deadline");
        }
        else {

        fetch(`api/Order/Edit_Order`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
              
                return (json)
            })
        sessionStorage.setItem('klient', this.refs.klient.value);
        sessionStorage.setItem('priority', this.refs.priority.value);
        sessionStorage.setItem('deadline', this.refs.deadline.value);
            this.props.history.push('/oneorder');
        }
    }
    cancelEditOrder = (event) => {
        //dane z cache usuwają się przy wyjściu ze zlecenia
        sessionStorage.removeItem('orderId');
        sessionStorage.removeItem('klient');
        sessionStorage.removeItem('deadline');
        sessionStorage.removeItem('priority');  
        this.props.history.push('/orderwarehouse');
        
    }

    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }
    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('orderManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div className="EditOrder">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Edit order</h1>
                    </div>
                    <div className="EditOrder_c">
                        <form>
                            <div className="form-group">

                                <label>Customer</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputKlient"
                                    placeholder={sessionStorage.getItem('klient')}
                                    defaultValue={sessionStorage.getItem('klient')}
                                    ref="klient"
                                />
                            </div>
                            <div className="form-group">
                                <label>Priority</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputPriority"
                                    placeholder={sessionStorage.getItem('priority')}
                                    defaultValue={sessionStorage.getItem('priority')}
                                    ref="priority"
                                />
                            </div>
                            <div className="form-group">
                                <label>Deadline</label>
                                <input
                                    type="date"
                                    className="form-control"
                                    id="inputWidth"
                                    //placeholder={sessionStorage.getItem('deadline')}
                                    //defaultValue={sessionStorage.getItem('deadline')}
                                    ref="deadline"
                                />
                            </div>
                            <div className="form-group">
                                <button type="submit" className="success_edit_order" onClick={this.handleEditOrder}>Edit order</button>

                                <button type="submit" className="danger_edit_order" onClick={this.cancelEditOrder}>Cancel</button>

                            </div>

                        </form>
                    </div>


                </div>

            );
        }
        else {
            return (
                <div className="HomePageFail">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback2} >Back to home page</button>
                </div>
            );
        }
    }
}