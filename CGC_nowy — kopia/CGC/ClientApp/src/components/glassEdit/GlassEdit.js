import React, { Component } from "react";
import './GlassEdit.css'
import Sidebar from '../Sidebar';

export class GlassEdit extends Component {
    displayName = GlassEdit.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
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
                console.log(json);
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
                console.log(json);
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

    handleGlassEdit = (event) => {
        event.preventDefault();
        const receiver = {
            glass: {
                type: this.refs.type.value,
                hight: this.refs.thickness.value,
                width: this.refs.width.value,
                length: this.refs.length.value,
                color: this.refs.color.value,
                owner: this.refs.owner.value,
                desk: sessionStorage.getItem('desk'),
                glass_Id: JSON.parse(sessionStorage.getItem('id')),
            },
            
                  
            
            user: {
                login: sessionStorage.getItem('login')
            }
        }


        fetch(`api/Magazine/Edit_Glass`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(receiver)
                console.log(json)
                return (json)
            })
        this.props.history.push('/glasswarehouse');

        console.log(receiver)
    }

    cancelGlassEdit = (event) => {
        sessionStorage.removeItem('length');
        sessionStorage.removeItem('width');
        sessionStorage.removeItem('thickness');
        sessionStorage.removeItem('color');
        sessionStorage.removeItem('type');
        sessionStorage.removeItem('amount');
        sessionStorage.removeItem('owner');
        this.props.history.push('/glasswarehouse');
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
    render() {
        let x = this.colorsSelector()
        let y = this.typeSelector()
        return (
            <div className="userchangeee">
                <Sidebar />
                <div className="userChange">
                    <form>
                        <div className="form-group">
                            <h2>Edycja szkła</h2>
                            <label>Długość</label>
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
                            <label>Szerokość</label>
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
                            <label>Typ</label>
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
                            <label>Grubość</label>
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
                            <label>Kolor</label>
                            <select
                                type="text"
                                className="form-control"
                                id="inputColor"
                                placeholder={sessionStorage.getItem('color')}
                                defaultValue={sessionStorage.getItem('color')}
                                ref="color"
                            >
                                <option selected={sessionStorage.getItem('color')}> { sessionStorage.getItem('color') } </option>
                                {x}
                            </select>
                        </div>
                        <div className="form-group">
                            <label>Właściciel</label>
                            <input
                                type="text"
                                className="form-control"
                                id="inputOwner"
                                placeholder={sessionStorage.getItem('owner')}
                                defaultValue={sessionStorage.getItem('owner')}
                                ref="owner"
                            />
                        </div>
                    
                        <div className="form-group">
                            <button type="submit" className="danger_glass_edit" onClick={this.cancelGlassEdit}>Anuluj</button>
                            <button type="submit" className="success_glass_edit" onClick={this.handleGlassEdit}>Zastosuj zmiany</button>
                  
                        </div>

                    </form>
                </div>
            </div>
        );
    }
}