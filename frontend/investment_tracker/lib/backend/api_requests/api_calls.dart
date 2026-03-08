import 'dart:convert';
import 'dart:typed_data';
import '../schema/structs/index.dart';

import 'package:flutter/foundation.dart';

import '/flutter_flow/flutter_flow_util.dart';
import 'api_manager.dart';

export 'api_manager.dart' show ApiCallResponse;

const _kPrivateApiFunctionName = 'ffPrivateApiCall';

class GetAssetsCall {
  static Future<ApiCallResponse> call({
    String? userId = '',
  }) async {
    return ApiManager.instance.makeApiCall(
      callName: 'GetAssets',
      apiUrl: 'https://investment-tracker-11jp.onrender.com/api/assets',
      callType: ApiCallType.GET,
      headers: {
        'Key': 'Authorization',
        'Value': 'Basic ZmY6dGVzdDEyMzQ=',
        'Accept': 'application/json',
        'userId': '${userId}',
      },
      params: {},
      returnBody: true,
      encodeBodyUtf8: false,
      decodeUtf8: false,
      cache: false,
      isStreamingApi: false,
      alwaysAllowBody: false,
    );
  }

  static List<int>? id(dynamic response) => (getJsonField(
        response,
        r'''$[:].id''',
        true,
      ) as List?)
          ?.withoutNulls
          .map((x) => castToType<int>(x))
          .withoutNulls
          .toList();
  static List<String>? code(dynamic response) => (getJsonField(
        response,
        r'''$[:].code''',
        true,
      ) as List?)
          ?.withoutNulls
          .map((x) => castToType<String>(x))
          .withoutNulls
          .toList();
  static List<String>? name(dynamic response) => (getJsonField(
        response,
        r'''$[:].name''',
        true,
      ) as List?)
          ?.withoutNulls
          .map((x) => castToType<String>(x))
          .withoutNulls
          .toList();
  static List<String>? category(dynamic response) => (getJsonField(
        response,
        r'''$[:].category''',
        true,
      ) as List?)
          ?.withoutNulls
          .map((x) => castToType<String>(x))
          .withoutNulls
          .toList();
}

class GetInvestmentsCall {
  static Future<ApiCallResponse> call({
    String? userId = '',
  }) async {
    return ApiManager.instance.makeApiCall(
      callName: 'GetInvestments',
      apiUrl: 'https://investment-tracker-11jp.onrender.com/api/investments',
      callType: ApiCallType.GET,
      headers: {
        'Accept': 'application/json',
        'userId': '${userId}',
      },
      params: {},
      returnBody: true,
      encodeBodyUtf8: false,
      decodeUtf8: false,
      cache: false,
      isStreamingApi: false,
      alwaysAllowBody: false,
    );
  }

  static List<InvestitieStruct>? investments(dynamic response) => (getJsonField(
        response,
        r'''$.investments''',
        true,
      ) as List?)
          ?.withoutNulls
          .map((x) => InvestitieStruct.maybeFromMap(x))
          .withoutNulls
          .toList();
  static int? id(dynamic response) => castToType<int>(getJsonField(
        response,
        r'''$.investments[:].id''',
      ));
  static String? assetCode(dynamic response) => castToType<String>(getJsonField(
        response,
        r'''$.investments[:].assetCode''',
      ));
  static double? quantity(dynamic response) => castToType<double>(getJsonField(
        response,
        r'''$.investments[:].quantity''',
      ));
  static double? buyPrice(dynamic response) => castToType<double>(getJsonField(
        response,
        r'''$.investments[:].buyPrice''',
      ));
  static String? buyDate(dynamic response) => castToType<String>(getJsonField(
        response,
        r'''$.investments[:].buyDate''',
      ));
  static double? currentPrice(dynamic response) =>
      castToType<double>(getJsonField(
        response,
        r'''$.investments[:].currentPrice''',
      ));
  static double? costBasis(dynamic response) => castToType<double>(getJsonField(
        response,
        r'''$.investments[:].costBasis''',
      ));
  static double? currentValue(dynamic response) =>
      castToType<double>(getJsonField(
        response,
        r'''$.investments[:].currentValue''',
      ));
  static double? pnlUsd(dynamic response) => castToType<double>(getJsonField(
        response,
        r'''$.investments[:].pnlUsd''',
      ));
  static double? pnPct(dynamic response) => castToType<double>(getJsonField(
        response,
        r'''$.investments[:].pnlPct''',
      ));
}

class PostInvestmentCall {
  static Future<ApiCallResponse> call({
    int? assetId,
    double? quantity,
    double? buyPrice,
    String? buyDate = '',
    String? userId = '',
  }) async {
    final ffApiRequestBody = '''
{
  "assetId": ${assetId},
  "quantity": ${quantity},
  "buyPrice": ${buyPrice},
  "buyDate": "${escapeStringForJson(buyDate)}"
}''';
    return ApiManager.instance.makeApiCall(
      callName: 'PostInvestment',
      apiUrl: 'https://investment-tracker-11jp.onrender.com/api/investments',
      callType: ApiCallType.POST,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'userId': '${userId}',
      },
      params: {},
      body: ffApiRequestBody,
      bodyType: BodyType.JSON,
      returnBody: true,
      encodeBodyUtf8: false,
      decodeUtf8: false,
      cache: false,
      isStreamingApi: false,
      alwaysAllowBody: false,
    );
  }

  static int? id(dynamic response) => castToType<int>(getJsonField(
        response,
        r'''$.id''',
      ));
  static String? asset(dynamic response) => castToType<String>(getJsonField(
        response,
        r'''$.asset''',
      ));
  static double? quantity(dynamic response) => castToType<double>(getJsonField(
        response,
        r'''$.quantity''',
      ));
  static double? buyPrice(dynamic response) => castToType<double>(getJsonField(
        response,
        r'''$.buyPrice''',
      ));
  static String? buyDate(dynamic response) => castToType<String>(getJsonField(
        response,
        r'''$.buyDate''',
      ));
  static double? costBasis(dynamic response) => castToType<double>(getJsonField(
        response,
        r'''$.costBasis''',
      ));
}

class GetPortfolioSummaryCall {
  static Future<ApiCallResponse> call({
    String? userId = '',
  }) async {
    return ApiManager.instance.makeApiCall(
      callName: 'GetPortfolioSummary',
      apiUrl:
          'https://investment-tracker-11jp.onrender.com/api/portfolio/summary',
      callType: ApiCallType.GET,
      headers: {
        'Accept': 'application/json',
        'userId': '${userId}',
      },
      params: {},
      returnBody: true,
      encodeBodyUtf8: false,
      decodeUtf8: false,
      cache: false,
      isStreamingApi: false,
      alwaysAllowBody: false,
    );
  }

  static double? totalCostBasis(dynamic response) =>
      castToType<double>(getJsonField(
        response,
        r'''$.totalCostBasis''',
      ));
  static double? totalCurrentValue(dynamic response) =>
      castToType<double>(getJsonField(
        response,
        r'''$.totalCurrentValue''',
      ));
  static double? totalPnlUsd(dynamic response) =>
      castToType<double>(getJsonField(
        response,
        r'''$.totalPnlUsd''',
      ));
  static double? totalPnlPercentage(dynamic response) =>
      castToType<double>(getJsonField(
        response,
        r'''$.totalPnlPercentage''',
      ));
}

class DeleteInvestmentCall {
  static Future<ApiCallResponse> call({
    int? id,
    String? userId = '',
  }) async {
    return ApiManager.instance.makeApiCall(
      callName: 'DeleteInvestment',
      apiUrl:
          'https://investment-tracker-11jp.onrender.com/api/investments/${id}',
      callType: ApiCallType.DELETE,
      headers: {
        'Accept': 'application/json',
        'userId': '${userId}',
      },
      params: {},
      returnBody: true,
      encodeBodyUtf8: false,
      decodeUtf8: false,
      cache: false,
      isStreamingApi: false,
      alwaysAllowBody: false,
    );
  }
}

class ApiPagingParams {
  int nextPageNumber = 0;
  int numItems = 0;
  dynamic lastResponse;

  ApiPagingParams({
    required this.nextPageNumber,
    required this.numItems,
    required this.lastResponse,
  });

  @override
  String toString() =>
      'PagingParams(nextPageNumber: $nextPageNumber, numItems: $numItems, lastResponse: $lastResponse,)';
}

String _toEncodable(dynamic item) {
  return item;
}

String _serializeList(List? list) {
  list ??= <String>[];
  try {
    return json.encode(list, toEncodable: _toEncodable);
  } catch (_) {
    if (kDebugMode) {
      print("List serialization failed. Returning empty list.");
    }
    return '[]';
  }
}

String _serializeJson(dynamic jsonVar, [bool isList = false]) {
  jsonVar ??= (isList ? [] : {});
  try {
    return json.encode(jsonVar, toEncodable: _toEncodable);
  } catch (_) {
    if (kDebugMode) {
      print("Json serialization failed. Returning empty json.");
    }
    return isList ? '[]' : '{}';
  }
}

String? escapeStringForJson(String? input) {
  if (input == null) {
    return null;
  }
  return input
      .replaceAll('\\', '\\\\')
      .replaceAll('"', '\\"')
      .replaceAll('\n', '\\n')
      .replaceAll('\t', '\\t');
}
