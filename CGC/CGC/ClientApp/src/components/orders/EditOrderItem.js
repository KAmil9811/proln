import React, { Component } from "react";
import './EditOrderItem.css';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';

export class EditOrderItem extends Component {
    displayName = EditOrderItem.name;
    constructor(props) {
        super(props);
        this.state = {
            value: sessionStorage.getItem('status'),
            colors: [],
            type: [],

        }
    }
    
    componentDidMount() {
        var table2 = [];
        var table3 = [];
        fetch(`api/Magazine/Return_All_Colors`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
              
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        color: json[i],
                    })
                };
                this.setState({
                    colors: table2

                });
            })

        fetch(`api/Magazine/Return_All_Type`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                
                for (var i = 0; i < json.length; i++) {
                    table3.push({
                        type: json[i],
                    })
                };
                this.setState({
                    type: table3

                });
            })
    }

    handleItemEdit = (event) => {
        event.preventDefault();
        const receiver = {
            item: {
                type: this.refs.type.value,
                width: this.refs.width.value,
                length: this.refs.length.value,
                color: this.refs.color.value,
                desk: sessionStorage.getItem('desk'),
                status: this.state.value,
                thickness: this.refs.thickness.value,
                id: sessionStorage.getItem('itemId')
            },
            user: {
                login: sessionStorage.getItem('login')
            },
            order:
            {
                id_order: sessionStorage.getItem('orderId')
            }
        }


        fetch(`api/Order/Edit_Order_Items`, {
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
            .then(json => {
                this.props.history.push('/oneorder');
            })   
        

       // console.log(receiver)
    }

    cancelItemEdit = (event) => {
        sessionStorage.removeItem('length');
        sessionStorage.removeItem('width');
        sessionStorage.removeItem('thickness');
        sessionStorage.removeItem('color');
        sessionStorage.removeItem('type');
        sessionStorage.removeItem('status');
        sessionStorage.removeItem('desk');
        sessionStorage.removeItem('itemId');
        this.props.history.push('/oneorder');
    }


    colorsSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.colors.length; i++) {

            tab.push(< option value={this.state.colors[i].color} > {this.state.colors[i].color}</option >)


        }
        return (tab)
    }
    typeSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.type.length; i++) {

            tab.push(< option value={this.state.type[i].type} > {this.state.type[i].type}</option >)


        }
        return (tab)
    }

    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    render() {
        let x = this.colorsSelector()
        let y = this.typeSelector()
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
                <div>
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Edit items</h1>
                    </div>
                    <div className="userChange">
                        <form>
                            <div className="form-group">
                                <label>Length</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    id="inputLength"
                                    placeholder={sessionStorage.getItem('length')}
                                    defaultValue={sessionStorage.getItem('length')}
                                    ref="length"
                                />
                            </div>
                            <div className="form-group">
                                <label>Width</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    id="inputWidth"
                                    placeholder={sessionStorage.getItem('width')}
                                    defaultValue={sessionStorage.getItem('width')}
                                    ref="width"
                                />
                            </div>
                            <div className="form-group">
                                <label>Type</label>
                                <select
                                    type="text"
                                    className="form-control"
                                    placeholder={sessionStorage.getItem('type')}
                                    defaultValue={sessionStorage.getItem('type')}
                                    ref="type"
                                >
                                    <option selected={sessionStorage.getItem('type')}> {sessionStorage.getItem('type')} </option>
                                    {y}
                                </select>

                            </div>
                            <div className="form-group">
                                <label>Thickness</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    id="inputThickness"
                                    placeholder={sessionStorage.getItem('thickness')}
                                    defaultValue={sessionStorage.getItem('thickness')}
                                    ref="thickness"
                                />
                            </div>
                            <div className="form-group">
                                <label>Color</label>
                                <select
                                    type="text"
                                    className="form-control"
                                    id="inputColor"
                                    placeholder={sessionStorage.getItem('color')}
                                    defaultValue={sessionStorage.getItem('color')}
                                    ref="color"
                                >
                                    <option selected={sessionStorage.getItem('color')}> {sessionStorage.getItem('color')} </option>
                                    {x}
                                </select>
                            </div>
                            <div className="form-group">


                                <label>Status:</label><br />
                                <select onChange={(e) => {
                                    this.setState({ value: e.target.value });
                                   
                                }} >
                                    <option value={sessionStorage.getItem('status')}>{sessionStorage.getItem('status')}</option>
                                    <option value="awaiting">Oczekujące</option>
                                    <option value="ready">Gotowe</option>
                                    <option value="cut">W trakcie</option>
                                    <option value="deleted">Usunięte</option>
                                </select>
                            </div>


                            <div className="form-group">
                                <button type="submit" className="danger_edit_order_item" onClick={this.cancelItemEdit}>Cancel</button>
                                <Link to="/oneorder"><button type="submit" className="success_edit_order_item" onClick={this.handleItemEdit}>Edit item</button></Link>
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